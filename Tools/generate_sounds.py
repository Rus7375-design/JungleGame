"""
generate_sounds.py - synthesizes the new sound effects and the menu music
for the Assignment 12 sound pass. No samples, no downloads: every sound is
built from sine/saw/triangle waves and filtered noise, so it is original work.

Run:  python generate_sounds.py        (needs numpy and scipy)
Output: ./Audio/*.wav  (mono, 44.1 kHz, 16 bit)
"""
import os
import wave
import numpy as np
from scipy.signal import lfilter

SR = 44100
RNG = np.random.default_rng(12)        # fixed seed: same files every run
OUT = os.path.join(os.path.dirname(os.path.abspath(__file__)), "Audio")
os.makedirs(OUT, exist_ok=True)


# ---------- helpers ----------
def t_axis(sec):
    return np.arange(int(sec * SR)) / SR


def sweep(f0, f1, sec, expo=True):
    """phase-accurate frequency sweep from f0 to f1 Hz"""
    t = t_axis(sec)
    if expo:
        f = f0 * (f1 / f0) ** (t / sec)
    else:
        f = f0 + (f1 - f0) * (t / sec)
    return np.cumsum(f) / SR              # phase in cycles


def sine(ph):
    return np.sin(2 * np.pi * ph)


def saw(ph):
    return 2 * (ph % 1.0) - 1


def tri(ph):
    return 2 * np.abs(2 * (ph % 1.0) - 1) - 1


def square(ph):
    return np.sign(np.sin(2 * np.pi * ph))


def noise(sec):
    return RNG.uniform(-1, 1, int(sec * SR))


def lowpass(x, fc):
    a = np.exp(-2 * np.pi * fc / SR)
    return lfilter([1 - a], [1, -a], x)


def highpass(x, fc):
    return x - lowpass(x, fc)


def env_ad(n, attack, decay_pow=3.0):
    """quick attack then exponential-like decay"""
    a = max(1, int(attack * SR))
    e = np.ones(n)
    e[:a] = np.linspace(0, 1, a)
    d = np.linspace(0, 1, n - a)
    e[a:] = (1 - d) ** decay_pow
    return e


def fade(x, ms=4):
    """tiny fade in/out so a sound never starts or ends with a click"""
    n = int(ms / 1000 * SR)
    x = x.copy()
    x[:n] *= np.linspace(0, 1, n)
    x[-n:] *= np.linspace(1, 0, n)
    return x


def norm(x, peak=0.8):
    return x / (np.max(np.abs(x)) + 1e-9) * peak


def save(name, x):
    x = np.clip(x, -1, 1)
    pcm = (x * 32767).astype(np.int16)
    path = os.path.join(OUT, name)
    with wave.open(path, "wb") as w:
        w.setnchannels(1)
        w.setsampwidth(2)
        w.setframerate(SR)
        w.writeframes(pcm.tobytes())
    print(f"{name:22s} {len(x) / SR:5.2f} s   peak {np.max(np.abs(x)):.2f}")


# ---------- enemy wind-up sounds (they play BEFORE the attack lands) ----------
def windup_melee():
    """patrol: rising whoosh + warning tone, 0.5 s (the wind-up is 0.5 s)"""
    sec = 0.5
    n = int(sec * SR)
    tone = saw(sweep(180, 520, sec)) * 0.5 + sine(sweep(360, 1040, sec)) * 0.25
    whoosh = highpass(noise(sec), 600)
    swell = np.linspace(0.15, 1, n) ** 1.6
    x = (tone + whoosh * 0.5) * swell
    return fade(norm(x, 0.8), 3)


def windup_charge():
    """charger: heavy engine rev, low to high with vibrato, 0.7 s"""
    sec = 0.7
    n = int(sec * SR)
    t = t_axis(sec)
    vib = 1 + 0.04 * np.sin(2 * np.pi * 14 * t)
    ph = sweep(70, 340, sec) * vib
    body = saw(ph) * 0.6 + square(ph * 0.5) * 0.3
    rumble = lowpass(noise(sec), 300) * 0.8
    swell = np.linspace(0.2, 1, n) ** 1.3
    x = lowpass(body + rumble, 2500) * swell
    return fade(norm(x, 0.85), 4)


def windup_aim():
    """shooter: laser charge, rising beeps that speed up, 0.6 s"""
    sec = 0.6
    n = int(sec * SR)
    t = t_axis(sec)
    ph = sweep(500, 2200, sec)
    tone = sine(ph) * 0.7 + sine(ph * 2) * 0.2
    pulse_rate = 8 + 30 * (t / sec)       # pulses get faster
    pulse = 0.55 + 0.45 * np.sin(2 * np.pi * np.cumsum(pulse_rate) / SR)
    swell = np.linspace(0.3, 1, n)
    return fade(norm(tone * pulse * swell, 0.7), 3)


def bomb_warning():
    """falling bomb: low rumble that grows, 1.5 s (plays when the warning mark appears)"""
    sec = 1.5
    n = int(sec * SR)
    t = t_axis(sec)
    rumble = lowpass(noise(sec), 140) * 3.0
    hum = sine(sweep(48, 95, sec)) * 0.7 + sine(sweep(96, 190, sec)) * 0.25
    throb = 0.75 + 0.25 * np.sin(2 * np.pi * 6 * t)
    swell = (np.linspace(0, 1, n) ** 1.2) * np.linspace(1, 0.6, n)
    x = (rumble + hum) * throb * swell
    return fade(norm(x, 0.85), 20)


def boss_shot():
    """boss volley: heavy low shot, 0.28 s"""
    sec = 0.28
    n = int(sec * SR)
    ph = sweep(420, 90, sec)
    x = (saw(ph) * 0.6 + square(ph) * 0.3 + lowpass(noise(sec), 2500) * 0.4) * env_ad(n, 0.004, 3)
    return fade(norm(lowpass(x, 4000), 0.85), 2)


# ---------- enemy death ----------
def enemy_death(f0=520, f1=70, seed_shift=0):
    sec = 0.38
    n = int(sec * SR)
    ph = sweep(f0, f1, sec)
    blip = square(ph) * 0.45 + tri(ph) * 0.4
    burst = lowpass(noise(sec), 5000 - seed_shift * 800) * 0.7
    x = (blip + burst) * env_ad(n, 0.003, 2.2)
    return fade(norm(x, 0.85), 2)


# ---------- UI and game states ----------
def ui_click():
    sec = 0.07
    n = int(sec * SR)
    t = t_axis(sec)
    x = (sine(1200 * t) + 0.4 * sine(1800 * t)) * env_ad(n, 0.002, 4)
    return fade(norm(x, 0.6), 1)


def lose():
    """sad falling phrase, 1.3 s"""
    notes = [(392.0, 0.25), (329.6, 0.25), (261.6, 0.25), (196.0, 0.55)]
    parts = []
    for f, d in notes:
        n = int(d * SR)
        t = np.arange(n) / SR
        wave_ = tri(f * t) * 0.7 + sine(f * t * 0.5) * 0.25
        parts.append(wave_ * env_ad(n, 0.01, 1.6))
    x = np.concatenate(parts)
    return fade(norm(lowpass(x, 3500), 0.75), 5)


def hurt_2():
    """second hit sound, so the hit is not always the same clip"""
    sec = 0.32
    n = int(sec * SR)
    ph = sweep(300, 110, sec)
    x = (saw(ph) * 0.55 + lowpass(noise(sec), 3000) * 0.6) * env_ad(n, 0.003, 2.4)
    return fade(norm(lowpass(x, 4500), 0.85), 2)


# ---------- menu music (calm seamless loop) ----------
def menu_music():
    """
    72 BPM, 8 bars of 4/4, chords Am - F - C - G (2 bars each).
    Everything is rendered into a circular buffer, so tails wrap around
    to the start: the loop point has no click and no silence.
    """
    bpm = 72
    beat = int(60 / bpm * SR)             # 36750 samples, exact integer
    bar = beat * 4
    total = bar * 8
    buf = np.zeros(total)

    def add(start, sig):
        """mix sig into the circular buffer starting at 'start' (wraps)"""
        idx = (start + np.arange(len(sig))) % total
        np.add.at(buf, idx, sig)

    def midi(m):
        return 440.0 * 2 ** ((m - 69) / 12)

    # chord roots (MIDI) and chord tones
    chords = [
        ("Am", 57, [57, 60, 64]),
        ("F", 53, [53, 57, 60]),
        ("C", 48, [60, 64, 67]),
        ("G", 55, [55, 59, 62]),
    ]

    for ci, (_, root, tones) in enumerate(chords):
        start = ci * 2 * bar
        length = 2 * bar + beat * 2       # pad overlaps into the next chord
        n = length
        t = np.arange(n) / SR
        # soft pad: detuned triangles + sine, slow attack, slow release
        pad = np.zeros(n)
        for m in tones:
            f = midi(m)
            pad += tri(f * t) * 0.5 + tri(f * 1.004 * t) * 0.5 + sine(f * t) * 0.4
        a = int(1.2 * SR)
        r = int(1.6 * SR)
        e = np.ones(n)
        e[:a] = np.linspace(0, 1, a) ** 1.5
        e[-r:] = np.linspace(1, 0, r) ** 1.5
        pad = lowpass(pad * e, 1400) * 0.22
        add(start, pad)

        # bass: root note on beat 1 and beat 3 of every bar
        for b in range(2):
            for half in range(2):
                s = start + b * bar + half * 2 * beat
                nb = int(1.4 * beat)
                tb = np.arange(nb) / SR
                f = midi(root - 12)
                bass = (sine(f * tb) + 0.3 * sine(2 * f * tb)) * env_ad(nb, 0.01, 2.0)
                add(s, bass * 0.32)

        # arpeggio: soft plucks on every 8th note, pattern up and down
        pattern = [0, 1, 2, 1, 2, 1, 0, 1]
        for step in range(16):            # 2 bars * 8 eighths
            m = tones[pattern[step % 8]] + 12
            s = start + step * (beat // 2)
            npk = int(0.9 * beat)
            tp = np.arange(npk) / SR
            f = midi(m)
            pluck = (sine(f * tp) + 0.35 * sine(2 * f * tp)) * env_ad(npk, 0.004, 3.2)
            add(s, pluck * 0.12)

    # a soft melody note on the first beat of each chord
    melody = [(0, 72), (2 * bar, 69), (4 * bar, 67), (6 * bar, 71)]
    for s, m in melody:
        nm = int(3.0 * beat)
        tm = np.arange(nm) / SR
        f = midi(m)
        note = (sine(f * tm) + 0.25 * sine(2 * f * tm)) * env_ad(nm, 0.06, 1.5)
        add(s + beat, note * 0.16)

    # circular echo (np.roll keeps the loop seamless)
    echo = np.roll(buf, int(beat * 0.75)) * 0.28 + np.roll(buf, int(beat * 1.5)) * 0.14
    buf = buf + echo

    # remove DC offset and any very low rumble, then normalise
    buf = buf - np.mean(buf)
    buf = highpass(buf, 35)
    return norm(buf, 0.7)


# ---------- render everything ----------
if __name__ == "__main__":
    SFX_GAIN = 0.7     # new effects peak close to the existing ones (jump, hurt, shoot)

    def sfx(name, x):
        save(name, x * SFX_GAIN)

    sfx("windup_melee.wav", windup_melee())
    sfx("windup_charge.wav", windup_charge())
    sfx("windup_aim.wav", windup_aim())
    sfx("bomb_warning.wav", bomb_warning())
    sfx("boss_shot.wav", boss_shot())
    sfx("enemy_death.wav", enemy_death(520, 70, 0))
    sfx("enemy_death_2.wav", enemy_death(620, 95, 1))
    sfx("ui_click.wav", ui_click())
    sfx("lose.wav", lose())
    sfx("hurt_2.wav", hurt_2())
    save("menu_music.wav", menu_music())
