from template_creator import run_puzzle

def highest_joltage(batteries: list[int], nb_lit_batteries: int) -> int:
    lit_batteries = []
    for battery in range(nb_lit_batteries):
        start_idx = 0 if not lit_batteries else (lit_batteries[-1][0]+1)
        stop_idx = len(batteries) - (nb_lit_batteries-len(lit_batteries)) + 1
        lit_batteries.append(max(list(enumerate(batteries))[start_idx:stop_idx], key=lambda b: b[1]))
    return int("".join(str(b[1]) for b in lit_batteries))


def part1(puzzle_input: str) -> int:
    return sum(highest_joltage([int(b) for b in line], 2) for line in puzzle_input.splitlines())


def part2(puzzle_input: str) -> int:
    return sum(highest_joltage([int(b) for b in line], 12) for line in puzzle_input.splitlines())


def solve(puzzle_input: str) -> tuple[int, int]:
    part_one = part1(puzzle_input)
    part_two = part2(puzzle_input)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=False)
