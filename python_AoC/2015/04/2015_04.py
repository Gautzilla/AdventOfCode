from template_creator import run_puzzle
import hashlib


def find_hash_with_leading_zeroes(puzzle_input: str, nb_zeroes: int) -> int:
    output = 0
    while (
        str(hashlib.md5(f"{puzzle_input}{output}".encode()).hexdigest())[:nb_zeroes]
        != "0" * nb_zeroes
    ):
        output += 1
    return output


def part1(puzzle_input: str) -> any:
    return find_hash_with_leading_zeroes(puzzle_input=puzzle_input, nb_zeroes=5)


def part2(puzzle_input: str) -> any:
    return find_hash_with_leading_zeroes(puzzle_input=puzzle_input, nb_zeroes=6)


def solve(puzzle_input: str) -> tuple[any, any]:
    part_one = part1(puzzle_input)
    part_two = part2(puzzle_input)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=True)
