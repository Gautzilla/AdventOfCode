from template_creator import run_puzzle
from collections import namedtuple

Position = namedtuple("Position", "x y")
directions = {
    ">": Position(1, 0),
    "^": Position(0, 1),
    "v": Position(0, -1),
    "<": Position(-1, 0),
}


def deliver(instructions: str, position: Position, visited_houses: set) -> None:
    visited_houses.add(position)
    for c in instructions:
        direction = directions[c]
        position = Position(position.x + direction.x, position.y + direction.y)
        visited_houses.add(position)


def part1(puzzle_input: str) -> int:
    visited_houses = set()
    deliver(
        instructions=puzzle_input,
        position=Position(0, 0),
        visited_houses=visited_houses,
    )
    return len(visited_houses)


def part2(puzzle_input: str) -> int:
    visited_houses = set()
    deliver(
        instructions=puzzle_input[::2],
        position=Position(0, 0),
        visited_houses=visited_houses,
    )
    deliver(
        instructions=puzzle_input[1::2],
        position=Position(0, 0),
        visited_houses=visited_houses,
    )
    return len(visited_houses)


def solve(puzzle_input: str) -> tuple[any, any]:
    part_one = part1(puzzle_input)
    part_two = part2(puzzle_input)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=True)
