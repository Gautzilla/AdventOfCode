from template_creator import run_puzzle

DIRECTIONS = ((1, 0), (0, 1), (-1, 0), (0, -1))


def search(obstacles, run, runs, visited):
    position, run_hist, direction, cost = run
    visited.append((position, direction))
    run_hist = list(set(run_hist + [position]))
    for d in range(direction - 1, direction + 2):
        dir = DIRECTIONS[d % len(DIRECTIONS)]
        position2 = tuple(map(sum, zip(position, dir)))
        if position2 in obstacles:
            continue
        if (position2, d) in visited or (
            position2,
            (d + 2) % len(DIRECTIONS),
        ) in visited:
            continue
        cost2 = cost + 1000 if d != direction else cost
        runs.append((position2, run_hist, d, cost2 + 1))


def solve_puzzle(obstacles, start, end) -> tuple:
    runs = [(start, [start], 0, 0)]
    visited = []
    best_spots = {start}
    best_path_cost = None
    while len(runs) > 0:
        runs = sorted(runs, key=lambda r: r[-1])
        cheapest = runs.pop(0)
        if best_path_cost is not None and cheapest[-1] > best_path_cost:
            break
        if cheapest[0] == end:
            best_path_cost = cheapest[-1]
            best_spots |= set(cheapest[1])
        search(obstacles, cheapest, runs, visited)
    return best_path_cost, len(best_spots) + 1


def parse_grid(
    puzzle_input: str,
) -> tuple[set[tuple[int, int]], tuple[int, int], tuple[int, int]]:
    start = (0, 0)
    end = (0, 0)
    obstacles = set()
    for y, line in enumerate(puzzle_input.splitlines()):
        for x, c in enumerate(line):
            if c == "S":
                start = (x, y)
            if c == "E":
                end = (x, y)
            if c == "#":
                obstacles.add((x, y))
    return obstacles, start, end


def solve(puzzle_input: str) -> tuple[any, any]:
    obstacles, start, end = parse_grid(puzzle_input)

    part_one, part_two = solve_puzzle(obstacles, start, end)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=True)
