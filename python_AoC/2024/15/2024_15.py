from template_creator import run_puzzle

MOVEMENTS = {">": (1, 0), "v": (0, 1), "<": (-1, 0), "^": (0, -1)}


def move(piece, direction, grid) -> bool:
    xdir, ydir = map(sum, zip(piece[:2], direction))
    if (xdir, ydir) not in {(x, y) for x, y, _ in grid}:
        piece[0] = xdir
        piece[1] = ydir
        return True
    obstacle = [v for v in grid if v[0] == xdir and v[1] == ydir][0]
    if obstacle[2] == "#":
        return False
    if move(obstacle, direction, grid):
        piece[0] = xdir
        piece[1] = ydir
        return True


def move2(piece, direction, grid, pieces_to_move) -> None:
    if piece in [p for p, _ in pieces_to_move]:
        return
    xdir, ydir = map(sum, zip(piece[:2], direction))
    size = 0 if piece[2] == "@" else 1
    if (
        (xdir, ydir) not in {(x, y) for x, y, _ in grid}
        and (xdir - 1, ydir) not in {(x, y) for x, y, _ in grid}
        and (xdir + 1, ydir) not in {(x, y) for x, y, _ in grid}
    ):
        pieces_to_move.append([piece, True])
    else:
        obstacles = [
            v
            for v in grid
            if (v[0] in [xdir, xdir + size] or v[0] + 1 in [xdir, xdir + size])
            and v[1] == ydir
            and v != piece
        ]
        if any(obstacle[2] == "#" for obstacle in obstacles):
            pieces_to_move.append([piece, False])
        pieces_to_move.append([piece, True])
        for obstacle in obstacles:
            move2(obstacle, direction, grid, pieces_to_move)
    if piece[2] == "@":
        if not pieces_to_move:
            pieces_to_move.append((piece, True))
        if all(p[1] for p in pieces_to_move):
            if [(56, 26, "@"), True] in pieces_to_move:
                a = 1
            for p, _ in pieces_to_move:
                if p not in grid:
                    a = 1
                x2, y2 = map(sum, zip(p[:2], direction))
                grid[grid.index(p)] = (x2, y2, p[2])


def part1(grid, movements) -> any:
    robot = [v for v in grid if v[2] == "@"][0]
    for m in movements:
        m = MOVEMENTS[m]
        move(robot, m, grid)
    return sum(x + 100 * y for x, y, c in grid if c == "O")


def print_grid(grid):
    for y in range(max(y for x, y, c in grid) + 1):
        line = ""
        x = 0
        while x < max(x2 for x2, *_ in grid) + 1:
            if (x, y) in ((x2, y2) for x2, y2, c in grid):
                *_, c = [v for v in grid if v[0] == x and v[1] == y][0]
                if c == "@":
                    line += c
                    x += 1
                if c == "O":
                    line += "[]"
                    x += 2
                if c == "#":
                    line += "##"
                    x += 2
            else:
                line += "."
                x += 1
        print(line)
    print()


def part2(large_grid, movements) -> any:
    for m in movements:
        robot = [v for v in large_grid if v[2] == "@"][0]
        m = MOVEMENTS[m]
        move2(robot, m, large_grid, [])
    return sum(x + 100 * y for x, y, c in large_grid if c == "O")


def solve(puzzle_input: str) -> tuple[any, any]:
    puzzle_input = puzzle_input.split("\n\n")
    grid = [
        [x, y, c]
        for y, line in enumerate(puzzle_input[0].splitlines())
        for x, c in enumerate(line)
        if c != "."
    ]
    movements = [c for line in puzzle_input[1].splitlines() for c in line]
    part_one = part1(grid, movements)
    large_grid = []
    for y, line in enumerate(puzzle_input[0].splitlines()):
        for x, c in enumerate(line):
            if c == ".":
                continue
            if c == "@":
                large_grid.append([x * 2, y, c])
            if c == "#":
                large_grid.append([x * 2, y, c])
            if c == "O":
                large_grid.append([x * 2, y, c])
    part_two = part2(large_grid, movements)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=True)
