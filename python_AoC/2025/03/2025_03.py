from template_creator import run_puzzle

def highest_joltage(batteries: list[int], nb_lit_batteries: int) -> int:
    max_dozen_idx = batteries[:-1].index(max(batteries[:-1]))
    max_unit = max(batteries[max_dozen_idx + 1 :])
    return batteries[max_dozen_idx] * 10 + max_unit


def part1(puzzle_input: str) -> int:
    return sum(highest_joltage([int(b) for b in line], 2) for line in puzzle_input.splitlines())


def part2(puzzle_input: str) -> any:
    return ""


def solve(puzzle_input: str) -> tuple[int, any]:
    part_one = part1(puzzle_input)
    part_two = part2(puzzle_input)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=False)
