from template_creator import run_puzzle
import re

def is_valid(id: int, pattern: str) -> bool:
    return re.fullmatch(pattern=pattern, string=str(id)) is None

def parse_ids(ids_str: str) -> range:
    lower, upper = map(int,ids_str.split("-"))
    return range(lower, upper+1)

def sum_valid_ids(puzzle_input: str, regex_pattern: str) -> int:
    return sum(
        id for id_str in puzzle_input.split(",") for id in parse_ids(id_str) if not is_valid(id, pattern=regex_pattern)
    )

def part1(puzzle_input: str) -> int:
    return sum_valid_ids(puzzle_input=puzzle_input, regex_pattern=r"(\d+)\1")

def part2(puzzle_input: str) -> int:
    return sum_valid_ids(puzzle_input=puzzle_input, regex_pattern=r"(\d+)\1+")


def solve(puzzle_input: str) -> tuple[int, int]:
    part_one = part1(puzzle_input)
    part_two = part2(puzzle_input)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=False)
