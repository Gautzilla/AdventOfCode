from template_creator import read_input
from pandas import Timestamp, Timedelta

EQUATIONS = {
    1: [lambda x, y: x + y, lambda x, y: x * y],
    2: [lambda x, y: x + y, lambda x, y: x * y, lambda x, y: int(str(x) + str(y))],
}


def is_solvable(result: int, numbers: list[int], current: int, index: int, part: int):
    if len(numbers) == 0:
        return current == result
    if current > result:
        return False
    return any(
        is_solvable(result, numbers[1:], equation(current, numbers[0]), index + 1, part)
        for equation in EQUATIONS[part]
    )


def solve_part(equations: list[tuple[int, list[int]]], part: int):
    return sum(
        equation[0]
        for equation in equations
        if is_solvable(equation[0], equation[1][1:], equation[1][0], 1, part)
    )


def solve(puzzle_input: str) -> tuple[any, any]:
    results = [int(line.split(":")[0]) for line in puzzle_input.splitlines()]
    numbers = [
        [int(v) for v in line.split(":")[1].split()]
        for line in puzzle_input.splitlines()
    ]
    equations = list(zip(results, numbers))
    part_one = solve_part(equations, 1)
    part_two = solve_part(equations, 2)

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
