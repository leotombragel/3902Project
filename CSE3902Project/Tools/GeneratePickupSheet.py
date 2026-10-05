"""Generates Content/images/pickups.png, the 16x16-per-cell sheet for items the team's link_items_spritesheet.png
doesn't have (rupee, Triforce, key, heart). Drawn with dark outlines to match the Link-style item art.

Layout (cells are 16x16):
    row 0: rupee frames 0..3 (a glint sweeps across the gem)
    row 1: Triforce frames 0..3 (glow pulse)
    row 2: key frames 0..1 (sparkle) | heart frames 0..1 (flash)

Run from the project root:  python Tools/GeneratePickupSheet.py
"""
from PIL import Image

T = 16
CLEAR = (0, 0, 0, 0)


def c(r, g, b, a=255):
    return (r, g, b, a)


def new_cell():
    return Image.new("RGBA", (T, T), CLEAR)


def draw_ascii(cell, rows, palette, x0, y0):
    px = cell.load()
    for dy, row in enumerate(rows):
        for dx, ch in enumerate(row):
            if ch in palette:
                px[x0 + dx, y0 + dy] = palette[ch]


def outline(cell, color):
    """Surround every opaque pixel with a 1px outline (4-neighbour)."""
    px = cell.load()
    edge = []
    for y in range(T):
        for x in range(T):
            if px[x, y][3] != 0:
                continue
            for nx, ny in ((x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1)):
                if 0 <= nx < T and 0 <= ny < T and px[nx, ny][3] == 255:
                    edge.append((x, y))
                    break
    for x, y in edge:
        px[x, y] = color


def star(cell, x, y, big=False):
    """A small white sparkle."""
    px = cell.load()
    for dx, dy in ((0, 0),) + (((-1, 0), (1, 0), (0, -1), (0, 1)) if big else ()):
        if 0 <= x + dx < T and 0 <= y + dy < T:
            px[x + dx, y + dy] = c(255, 255, 255) if (dx, dy) == (0, 0) else c(220, 255, 230)


# ---------------------------------------------------------------- rupee (green, 8 wide x 13 tall)
RUPEE = [
    "...OO...",
    "..OggO..",
    ".OgGGdO.",
    "OgGGGGdO",
    "OgGGGGdO",
    "OgGGGGdO",
    "OgGGGGdO",
    "OgGGGGdO",
    "OgGGGGdO",
    "OgGGGGdO",
    ".OgGGdO.",
    "..OgdO..",
    "...OO...",
]
RUPEE_PAL = {"O": c(0, 56, 24), "g": c(150, 255, 150), "G": c(40, 200, 72), "d": c(8, 128, 52)}
GLINTS = [None, (2, 4, False), (3, 6, True), (5, 9, False)]  # position inside the rupee art

# ---------------------------------------------------------------- heart (13 wide x 11 tall)
HEART = [
    "..OOO...OOO..",
    ".OrrrO.OrrrO.",
    "OrwwrrOrrrrrO",
    "OrwrrrrrrrrdO",
    "OrrrrrrrrrrdO",
    ".OrrrrrrrrdO.",
    "..OrrrrrrdO..",
    "...OrrrrdO...",
    "....OrrdO....",
    ".....OdO.....",
    "......O......",
]
HEART_PAL = {"O": c(72, 0, 0), "r": c(228, 44, 28), "d": c(150, 12, 12), "w": c(255, 200, 190)}
HEART_FLASH_PAL = {"O": c(72, 0, 0), "r": c(255, 96, 96), "d": c(200, 40, 40), "w": c(255, 255, 255)}

GOLD_DK, GOLD, GOLD_LT, GOLD_HI = c(176, 104, 0), c(240, 184, 24), c(255, 224, 88), c(255, 248, 176)
EDGE = c(88, 44, 0)


def make_rupee(frame):
    cell = new_cell()
    draw_ascii(cell, RUPEE, RUPEE_PAL, 4, 1)
    glint = GLINTS[frame]
    if glint:
        star(cell, 4 + glint[0], 1 + glint[1], glint[2])
    return cell


def make_triforce(frame):
    shade = [GOLD, GOLD_LT, GOLD_HI, GOLD_LT][frame]
    cell = new_cell()
    px = cell.load()
    # three triangles (7 wide, 5 tall, single-pixel apex). The top one sits on the middle; each bottom one has its
    # apex touching a corner of the top triangle's base, leaving an upside-down triangle of empty space between them.
    widths = [1, 3, 3, 5, 7]
    for center, top_y in ((7, 3), (4, 8), (10, 8)):
        for r, w in enumerate(widths):
            for x in range(center - w // 2, center + w // 2 + 1):
                px[x, top_y + r] = shade
    # simple shading: right edge of each triangle row darker, left edge brighter
    for y in range(T):
        row = [x for x in range(T) if px[x, y][3] == 255]
        for x in row:
            if x + 1 >= T or px[x + 1, y][3] == 0:
                px[x, y] = GOLD_DK if frame != 2 else GOLD
            elif x - 1 < 0 or px[x - 1, y][3] == 0:
                px[x, y] = GOLD_HI
    outline(cell, EDGE)
    if frame in (1, 3):
        star(cell, 7 if frame == 1 else 3, 3 if frame == 1 else 11)
    return cell


def make_key(frame):
    cell = new_cell()
    px = cell.load()
    # ring (hollow circle), shaft, two teeth
    for y in range(1, 8):
        for x in range(4, 12):
            d = (x - 7.5) ** 2 + (y - 4) ** 2
            if 1.6 ** 2 <= d <= 3.6 ** 2:
                px[x, y] = GOLD
    for y in range(7, 15):
        for x in (7, 8):
            px[x, y] = GOLD
    for y in (11, 12, 14):
        for x in (9, 10):
            px[x, y] = GOLD
    for y in range(T):
        for x in range(T):
            if px[x, y][3] == 255:
                left_clear = x - 1 < 0 or px[x - 1, y][3] == 0
                right_clear = x + 1 >= T or px[x + 1, y][3] == 0
                if left_clear:
                    px[x, y] = GOLD_HI
                elif right_clear:
                    px[x, y] = GOLD_DK
    outline(cell, EDGE)
    if frame == 1:
        star(cell, 5, 2)
    return cell


def make_heart(frame):
    cell = new_cell()
    draw_ascii(cell, HEART, HEART_FLASH_PAL if frame == 1 else HEART_PAL, 1, 3)
    return cell


sheet = Image.new("RGBA", (T * 4, T * 3), CLEAR)
for f in range(4):
    sheet.paste(make_rupee(f), (f * T, 0))
    sheet.paste(make_triforce(f), (f * T, T))
for f in range(2):
    sheet.paste(make_key(f), (f * T, 2 * T))
    sheet.paste(make_heart(f), ((2 + f) * T, 2 * T))
sheet.save("Content/images/pickups.png")
print("wrote Content/images/pickups.png", sheet.size)
