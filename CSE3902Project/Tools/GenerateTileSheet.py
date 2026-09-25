"""Generates Content/images/tiles.png, a 16x16-per-cell tile sheet in an NES Zelda dungeon style.

Layout (cells are 16x16, row 0 then row 1):
    row 0: ground | brick | step | pipe
    row 1: question frame 0..3 (animated)

Run from the project root:  python Tools/GenerateTileSheet.py
"""
from PIL import Image

T = 16
img = Image.new("RGBA", (T * 4, T * 2), (0, 0, 0, 0))
px = img.load()


def fill(cx, cy, x0, y0, x1, y1, color):
    """Fill an inclusive rectangle given in cell-local coordinates."""
    for y in range(y0, y1 + 1):
        for x in range(x0, x1 + 1):
            px[cx * T + x, cy * T + y] = color


def dot(cx, cy, x, y, color):
    px[cx * T + x, cy * T + y] = color


# Palette
SAND, SAND_DK, SAND_LT = (216, 168, 88, 255), (160, 112, 40, 255), (248, 216, 136, 255)
BLUE, BLUE_DK, BLUE_LT = (32, 64, 208, 255), (0, 0, 96, 255), (104, 144, 255, 255)
STONE, STONE_LT, STONE_DK = (216, 184, 120, 255), (255, 240, 200, 255), (112, 80, 32, 255)
PIPE, PIPE_DK, PIPE_LT = (72, 152, 232, 255), (16, 72, 168, 255), (160, 208, 255, 255)  # medium-light blue
GOLD, WHITE = (248, 184, 0, 255), (255, 255, 255, 255)


def draw_brick(cx, cy):
    fill(cx, cy, 0, 0, 15, 15, BLUE_DK)                 # mortar
    for row, y in enumerate((0, 4, 8, 12)):
        offset = 0 if row % 2 == 0 else 4               # stagger courses
        for x in range(-offset, 16, 8):
            x0, x1 = max(x + 1, 0), min(x + 7, 15)
            fill(cx, cy, x0, y + 1, x1, y + 3, BLUE)
            fill(cx, cy, x0, y + 1, x1, y + 1, BLUE_LT)  # top highlight


# ground: sand with speckles, lit top edge, dark bottom edge
fill(0, 0, 0, 0, 15, 15, SAND)
fill(0, 0, 0, 0, 15, 1, SAND_LT)
fill(0, 0, 0, 14, 15, 15, SAND_DK)
for x, y in ((2, 5), (9, 4), (13, 7), (5, 9), (11, 11), (3, 12), (7, 6)):
    dot(0, 0, x, y, SAND_DK)

# brick: dungeon border
draw_brick(1, 0)

# step: beveled carved stone block
fill(2, 0, 0, 0, 15, 15, STONE_DK)
fill(2, 0, 0, 0, 14, 14, STONE_LT)
fill(2, 0, 1, 1, 14, 14, STONE)
fill(2, 0, 4, 4, 11, 11, STONE_DK)
fill(2, 0, 5, 5, 11, 11, STONE_LT)
fill(2, 0, 5, 5, 10, 10, STONE)

# pipe: blue, rim on top, shaded cylinder body
fill(3, 0, 0, 0, 15, 15, (0, 0, 0, 0))
fill(3, 0, 0, 0, 15, 4, PIPE_DK)
fill(3, 0, 1, 1, 14, 3, PIPE)
fill(3, 0, 2, 1, 4, 3, PIPE_LT)
fill(3, 0, 1, 5, 14, 15, PIPE_DK)
fill(3, 0, 2, 5, 13, 15, PIPE)
fill(3, 0, 3, 5, 5, 15, PIPE_LT)
fill(3, 0, 11, 5, 13, 15, PIPE_DK)

# question block: brick body, gold plate, animated "?"
QUESTION = ("01110", "10001", "00001", "00110", "00100", "00000", "00100")
MARK_COLORS = (WHITE, (255, 240, 120, 255), (248, 152, 0, 255), (255, 240, 120, 255))
for frame in range(4):
    draw_brick(frame, 1)
    fill(frame, 1, 1, 1, 14, 14, BLUE_DK)
    fill(frame, 1, 2, 2, 13, 13, GOLD)
    fill(frame, 1, 2, 2, 13, 2, SAND_LT)
    fill(frame, 1, 2, 13, 13, 13, SAND_DK)
    for r, line in enumerate(QUESTION):
        for c, bit in enumerate(line):
            if bit == "1":
                dot(frame, 1, 5 + c + 1, 4 + r, MARK_COLORS[frame])

img.save("Content/images/tiles.png")
print("wrote Content/images/tiles.png", img.size)
