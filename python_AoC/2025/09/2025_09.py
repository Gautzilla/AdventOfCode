from template_creator import run_puzzle

vertical_edges = {}
horizontal_edges = {}

def is_in_rectangle(tile: tuple[int,int], r1: tuple[int,int], r2: tuple[int,int]) -> bool:
    rx = sorted((r1[0], r2[0]))
    ry = sorted((r1[1], r2[1]))
    return rx[0] <= tile[0] <= rx[1] and ry[0] <= tile[1] <= ry[1]

def is_edge_in_rectangle(tile1: tuple[int,int], tile2: tuple[int,int]) -> bool:
    """If an edge goes within the rectangle (that is not just on its edge), it is invalid.
    This considers that there will be no adjacent edges..."""
    x0,y0 = (min(tile1[0], tile2[0]),min(tile1[1], tile2[1]))
    x1,y1 = (max(tile1[0], tile2[0]),max(tile1[1], tile2[1]))
    for x,rs in vertical_edges.items():
        if not x0 < x < x1:
            continue
        for r in rs:
            if y0+1 in r or y1-1 in r:
                return True
    for y,rs in horizontal_edges.items():
        if not y0 < y < y1:
            continue
        for r in rs:
            if x0+1 in r or x1-1 in r:
                return True
    return False

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

def part2(tiles) -> int:
    max_area = 0
    for idx, tile in enumerate(tiles):
        for tile2 in tiles[idx+1:]:
            if is_edge_in_rectangle(tile, tile2):
                continue
            max_area = max(max_area, area(tile, tile2))
    return max_area

def add_edge(tile1, tile2) -> None:
    """Parse all outer edges of the surface."""
    if (x:=tile1[0]) == tile2[0]:
        r = range(min(tile1[1],tile2[1]), max(tile1[1],tile2[1])+1)
        if x in vertical_edges:
            vertical_edges[x].append(r)
        else:
            vertical_edges[x] = [r]
    if (y:=tile1[1]) == tile2[1]:
        r = range(min(tile1[0],tile2[0]), max(tile1[0],tile2[0])+1)
        if y in horizontal_edges:
            horizontal_edges[y].append(r)
        else:
            horizontal_edges[y] = [r]

def parse_tiles(puzzle_input: str) -> list[tuple[int,int]]:
    tiles = []
    for line in puzzle_input.splitlines():
        x,y = map(int, line.split(",", maxsplit=1))
        if tiles:
            add_edge((x,y),tiles[-1])
        tiles.append((x,y))
    add_edge(tiles[0],tiles[-1])
    return tiles

def solve(puzzle_input: str) -> tuple[int, int]:
    tiles = parse_tiles(puzzle_input)
    part_one = part1(tiles)
    part_two = part2(tiles)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=False)
