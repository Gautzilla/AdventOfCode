from template_creator import read_input
from pandas import Timestamp, Timedelta


def part1(puzzle_input: str) -> int:
    return puzzle_input.count("(") - puzzle_input.count(")")


def part2(puzzle_input: str) -> int:
    position = 0
    for i, c in enumerate(puzzle_input):
        i += 1
        position += 1 if c == "(" else -1
        if position == -1:
            return i


def solve(puzzle_input: str) -> tuple[any, any]:
    part_one = part1(puzzle_input)
    part_two = part2(puzzle_input)

    return part_one, part_two


if __name__ == "__main__":
    i = read_input(example=False)

    t_start = Timestamp.now()
    p1, p2 = solve(puzzle_input=i)
    t_stop = Timestamp.now()
    t = Timedelta(t_stop - t_start).total_seconds()

    print(f"{'Part one':<20}{p1:>10}")
    print(f"{'Part two':<20}{p2:>10}")
    print(f"\nSolved in {t} second{'s' if t >= 2 else ''}.")
