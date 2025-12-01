from template_creator import run_puzzle


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
    run_puzzle(solve_function=solve, example=True)
