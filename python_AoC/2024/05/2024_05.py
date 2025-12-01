from template_creator import read_input
from pandas import Timestamp, Timedelta
from functools import cmp_to_key

rules = {}


def compare(page1: int, page2: int):
    return 0 if (page1, page2) not in rules else rules[(page1, page2)]


def parse_rules(rules_input: list[str]) -> None:
    for rule in rules_input:
        x, y = [int(i) for i in rule.split("|")]
        rules[(x, y)] = -1
        rules[(y, x)] = 1


def parse_updates(updates: list[str]) -> list[list[int]]:
    return [[int(i) for i in update.split(",")] for update in updates]


def part1(updates) -> int:
    return sum(
        update[len(update) // 2]
        for update in updates
        if update == sorted(update, key=cmp_to_key(compare))
    )


def part2(updates) -> any:
    output = 0
    for update in updates:
        if (s := sorted(update, key=cmp_to_key(compare))) != update:
            output += s[len(s) // 2]
    return output


def solve(puzzle_input: str) -> tuple[any, any]:
    parse_rules([line for line in puzzle_input.splitlines() if "|" in line])
    updates = parse_updates([line for line in puzzle_input.splitlines() if "," in line])
    part_one = part1(updates)
    part_two = part2(updates)

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
