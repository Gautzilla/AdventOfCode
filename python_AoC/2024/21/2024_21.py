from template_creator import run_puzzle
from functools import cache

MAIN_KEYBOARD = {
    "0": ["", "^<", "^", "^>", "^^<", "^^", "^^>", "^^^<", "^^^", "^^^>", ">"],
    "1": [">v", "", ">", ">>", "^", "^>", "^>>", "^^", "^^>", "^^>>", ">>v"],
    "2": ["v", "<", "", ">", "<^", "^", "^>", "<^^", "^^", "^^>", "v>"],
    "3": ["<v", "<<", "<", "", "<<^", "<^", "^", "<<^^", "<^^", "^^", "v"],
    "4": [">vv", "v", "v>", "v>>", "", ">", ">>", "^", "^>", "^>>", ">>vv"],
    "5": ["vv", "<v", "v", "v>", "<", "", ">", "<^", "^", "^>", "vv>"],
    "6": ["<vv", "<<v", "<v", "v", "<<", "<", "", "<<^", "<^", "^", "vv"],
    "7": [">vvv", "vv", "vv>", "vv>>", "v", "v>", "v>>", "", ">", ">>", ">>vvv"],
    "8": ["vvv", "<vv", "vv", "vv>", "<v", "v", "v>", "<", "", ">", "vvv>"],
    "9": ["<vvv", "<<vv", "<vv", "vv", "<<v", "<v", "v", "<<", "<", "", "vvv"],
    "A": ["<", "^<<", "<^", "^", "^^<<", "<^^", "^^", "^^^<<", "<^^^", "^^^", ""],
}

DIR_KEYBOARD = {
    "^": {"^": "", ">": "v>", "v": "v", "<": "v<", "A": ">"},
    ">": {"^": "<^", ">": "", "v": "<", "<": "<<", "A": "^"},
    "v": {"^": "^", ">": ">", "v": "", "<": "<", "A": "^>"},
    "<": {"^": ">^", ">": ">>", "v": ">", "<": "", "A": ">>^"},
    "A": {"^": "<", ">": "v", "v": "<v", "<": "v<<", "A": ""},
}


@cache
def path_length(a, b, depth):
    if depth == 0:
        return len(DIR_KEYBOARD[a][b] + "A")
    next_sequence = DIR_KEYBOARD[a][b]
    return sum(
        path_length(*p, depth - 1)
        for p in zip("A" + next_sequence, next_sequence + "A")
    )


def drive_robots(puzzle_input: str, nb_robots: int) -> int:
    output = {}
    for input in puzzle_input.splitlines():
        sequence = ""
        cc = "A"
        for c in input:
            i = int(c) if c.isdigit() else 10
            sequence += MAIN_KEYBOARD[cc][i] + "A"
            cc = c
        output[input] = sum(
            path_length(*p, nb_robots - 1) for p in zip("A" + sequence, sequence)
        )
    return sum(int(key[:-1]) * value for key, value in output.items())


def part1(puzzle_input: str) -> any:
    return drive_robots(puzzle_input, 2)


def part2(puzzle_input: str) -> any:
    return drive_robots(puzzle_input, 25)


def solve(puzzle_input: str) -> tuple[any, any]:
    part_one = part1(puzzle_input)
    part_two = part2(puzzle_input)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=True)
