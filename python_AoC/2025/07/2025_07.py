from template_creator import run_puzzle

splitters = set()
grid_size = dict()

def move_down(tachyon: tuple[int,int]) -> tuple[int,int]:
    return tachyon[0], tachyon[1]+1

def split(tachyon: tuple[int,int]) -> set[tuple[int,int]]:
    candidates = {(tachyon[0]-1,tachyon[1]), (tachyon[0]+1,tachyon[1])}
    return {new for new in candidates if is_in_grid(new)}

def is_in_grid(tachyon: tuple[int,int]) -> bool:
    return 0 <= tachyon[0] < grid_size["x"] and 0 <= tachyon[1] < grid_size["y"]

def part1(start: tuple[int,int]) -> int:
    visited = set()
    running = {start}
    nb_splits = 0

    while running:
        tachyon = move_down(running.pop())
        if not is_in_grid(tachyon):
            continue
        if tachyon in visited:
            continue
        if tachyon in splitters:
            splitted = split(tachyon)
            visited |= splitted
            running |= splitted
            nb_splits+=1
            continue
        visited.add(tachyon)
        running.add(tachyon)
    """
    for y in range(grid_size["y"]):
        s = ""
        for x in range(grid_size["x"]):
            p = (x,y)
            if p in visited:
                s+="|"
            elif p == start:
                s+="S"
            elif p in splitters:
                s+="^"
            else:
                s += "."
        print(s)
    """
    return nb_splits

def part2(start: tuple[int,int]) -> any:
    visited = set()
    running = set()

    return ""

def parse_input(puzzle_input: str) -> tuple[int,int]:
    start = (0,0)
    for y, line in enumerate(puzzle_input.splitlines()):
        for x, c in enumerate(line):
            if c == "S":
                start = (x,y)
            if c == "^":
                splitters.add((x,y))
    return start


def solve(puzzle_input: str) -> tuple[any, any]:
    start = parse_input(puzzle_input)
    grid_size["x"] = len(puzzle_input.splitlines()[0])
    grid_size["y"]=len(puzzle_input.splitlines())
    part_one = part1(start)
    part_two = part2(start)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=False)
