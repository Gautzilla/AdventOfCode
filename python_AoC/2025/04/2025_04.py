from template_creator import run_puzzle


def neighbours(coords: tuple[int, int]) -> set[tuple[int, int]]:
    directions = ((-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1))
    return set((coords[0] + x, coords[1] + y) for x, y in directions)


def nb_neighbours(coords: tuple[int, int], rolls_coords: set[tuple[int, int]]) -> int:
    return sum(1 for neighbour in neighbours(coords) if neighbour in rolls_coords)


def is_movable(coords: tuple[int, int], rolls_coords: set[tuple[int, int]]) -> bool:
    return nb_neighbours(coords, rolls_coords) < 4


def part1(rolls_coords: set[tuple[int, int]]) -> int:
    return sum(1 for roll in rolls_coords if is_movable(roll, rolls_coords))


def part2(rolls_coords: set[tuple[int, int]]) -> int:
    removed_rolls = set()
    while rolls_to_remove := set(
        roll for roll in rolls_coords if is_movable(roll, rolls_coords)
    ):
        removed_rolls |= rolls_to_remove
        rolls_coords -= rolls_to_remove
    return sum(1 for _ in removed_rolls)


def solve(puzzle_input: str) -> tuple[int, int]:
    rolls_coords = set()
    for y, line in enumerate(puzzle_input.splitlines()):
        for x, char in enumerate(line):
            if char == "@":
                rolls_coords.add((x, y))
    part_one = part1(rolls_coords)
    part_two = part2(rolls_coords)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=False)
