from template_creator import run_puzzle
from functools import reduce


def surface(faces: list[int]) -> int:
    side_ares = [2 * faces[i] * faces[i - 1] for i in range(3)]
    return sum(side_ares) + min(side_ares) // 2


def ribbon_length(faces: list[int]) -> int:
    wrap = sum(2 * f for f in sorted(faces)[:-1])
    bow = reduce(lambda x, y: x * y, faces)
    return wrap + bow


def part1(puzzle_input) -> int:
    return sum(surface(box) for box in puzzle_input)


def part2(puzzle_input) -> int:
    return sum(ribbon_length(box) for box in puzzle_input)


def solve(puzzle_input: str) -> tuple[any, any]:
    puzzle_input = list(
        [int(dimension) for dimension in line.split("x")]
        for line in puzzle_input.split()
    )
    part_one = part1(puzzle_input)
    part_two = part2(puzzle_input)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=True)
