from template_creator import read_input
from pandas import Timestamp, Timedelta
import re


def part1(machines) -> any:
    return sum(solve_machine(machine) for machine in machines)


def part2(machines) -> any:
    prize_offset = 10_000_000_000_000
    machines = [
        (xa, ya, xb, yb, xp + prize_offset, yp + prize_offset)
        for xa, ya, xb, yb, xp, yp in machines
    ]
    return sum(solve_machine(machine) for machine in machines)


def solve_machine(machine):
    xa, ya, xb, yb, xp, yp = machine
    nb = (yp * xa - xp * ya) / (yb * xa - xb * ya)
    na = (xp - nb * xb) / xa
    return int((3 * na + nb)) if (nb.is_integer() and na.is_integer()) else 0


def parse_machine(lines: list[str]):
    xa, ya = [int(r) for r in re.search(r"A: X\+(\d+), Y\+(\d+)", lines[0]).groups()]
    xb, yb = [int(r) for r in re.search(r"B: X\+(\d+), Y\+(\d+)", lines[1]).groups()]
    xp, yp = [int(r) for r in re.search(r"X=(\d+), Y=(\d+)", lines[2]).groups()]
    return xa, ya, xb, yb, xp, yp


def solve(puzzle_input: str) -> tuple[any, any]:
    input_lines = [line for line in puzzle_input.splitlines() if line]
    machines = [
        parse_machine(input_lines[n : n + 3]) for n in range(0, len(input_lines), 3)
    ]
    part_one = part1(machines)
    part_two = part2(machines)

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
