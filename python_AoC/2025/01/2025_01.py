from template_creator import read_input
from pandas import Timestamp, Timedelta


def delta_dial(instruction: str) -> int:
    direction, distance = instruction[0], int(instruction[1:])
    direction = -1 if direction == "L" else 1
    return direction * distance


def solve(puzzle_input: str) -> tuple[int, int]:
    part_one = 0
    part_two = 0
    pointer = 50

    for line in puzzle_input.splitlines():
        previous = pointer
        pointer += delta_dial(line)

        if not pointer % 100:
            part_one += 1

        part_two += abs(pointer) // 100
        if pointer <= 0 < previous:
            part_two += 1

        pointer = (pointer + 100) % 100

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
