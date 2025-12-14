from template_creator import run_puzzle

def is_in_rectangle(tile: tuple[int,int], r1: tuple[int,int], r2: tuple[int,int]) -> bool:
    rx = sorted((r1[0], r2[0]))
    ry = sorted((r1[1], r2[1]))
    return rx[0] <= tile[0] <= rx[1] and ry[0] <= tile[1] <= ry[1]

def area(tile1: tuple[int,int], tile2: tuple[int,int]) -> int:
    x = abs(tile1[0] - tile2[0]) + 1
    y = abs(tile1[1] - tile2[1]) + 1
    return x*y

def part1(tiles: list[tuple[int,int]]) -> int:
    max_area = 0
    for idx, tile in enumerate(tiles):
        for tile2 in tiles[idx+1:]:
            max_area = max(max_area, area(tile, tile2))
    return max_area


def part2(puzzle_input: str) -> any:
    return ""

def parse_tiles(puzzle_input: str) -> list[tuple[int,int]]:
    tiles = []
    for line in puzzle_input.splitlines():
        tiles.append(tuple(map(int, line.split(",", maxsplit=1))))
    return tiles

def solve(puzzle_input: str) -> tuple[int, int]:
    tiles = parse_tiles(puzzle_input)
    part_one = part1(tiles)
    part_two = part2(puzzle_input)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=False)
