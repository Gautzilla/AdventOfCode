from template_creator import run_puzzle


def is_safe_one_direction(
    reports: list[int], condition: callable, nb_skips: int = 0
) -> bool:
    if nb_skips < 0:
        return False
    for index, level in list(enumerate(reports))[1:]:
        delta = reports[index] - reports[index - 1]
        if not condition(delta):
            updated_reports = [reports[:index] + reports[index + 1 :]]
            if index >= 1:
                updated_reports.append(reports[: index - 1] + reports[index:])
            if index < len(reports) - 1:
                updated_reports.append(reports[: index + 1] + reports[index + 2 :])
            return any(is_safe(updated, nb_skips - 1) for updated in updated_reports)
    return True


def is_safe(reports: list[int], nb_skips: int = 0) -> bool:
    conditions = [lambda delta: -3 <= delta <= -1, lambda delta: 1 <= delta <= 3]
    return any(
        is_safe_one_direction(reports, condition, nb_skips) for condition in conditions
    )


def part1(reports: list[list[int]]) -> any:
    return sum(1 for report in reports if is_safe(report))


def part2(reports: list[list[int]]) -> any:
    return sum(1 for report in reports if is_safe(report, nb_skips=1))


def solve(puzzle_input: str) -> tuple[any, any]:
    reports = [[int(r) for r in line.split()] for line in puzzle_input.splitlines()]
    part_one = part1(reports)
    part_two = part2(reports)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=True)
