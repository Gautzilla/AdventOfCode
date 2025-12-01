from template_creator import run_puzzle


def add_stone(stones: dict, stone: int, count: int):
    if stone in stones:
        stones[stone] += count
    else:
        stones[stone] = count


def blink_stone(stone: int) -> list[int]:
    if stone == 0:
        return [1]
    if not len(s := str(stone)) % 2:
        return [int(s[: len(s) // 2]), int(s[len(s) // 2 :])]
    return [2024 * stone]


def blink(stones: dict[int, int]) -> any:
    next_stones = {}

    for stone, count in stones.items():
        for replacement in blink_stone(stone):
            add_stone(next_stones, replacement, count)
    return next_stones


def solve_parts(stones: dict[int, int], nb_blinks: int) -> int:
    for _ in range(nb_blinks):
        stones = blink(stones)
    return sum(stones.values())


def solve(puzzle_input: str) -> tuple[any, any]:
    starting_stones = [int(s) for s in puzzle_input.split(" ")]
    stones = {}
    for stone in starting_stones:
        add_stone(stones, stone, 1)

    part_one = solve_parts(stones, nb_blinks=25)
    part_two = solve_parts(stones, nb_blinks=75)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=True)
