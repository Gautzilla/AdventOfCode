from scipy.cluster.hierarchy import complete

from template_creator import run_puzzle

cache = {}


def search(towels, design):
    if design == "":
        return 1
    if design in cache:
        return cache[design]
    for towel in towels:
        if design.startswith(towel) and search(towels, design[len(towel) :]):
            adds = search(towels, design[len(towel) :])
            if design in cache:
                cache[design] += adds
            else:
                cache[design] = adds
    if design not in cache:
        cache[design] = 0
    return 0 if design not in cache else cache[design]


def part1(towels, designs) -> any:
    return len([design for design in designs if search(list(towels), design)])


def part2(towels, designs) -> any:
    return sum(search(list(towels), design) for design in designs)


def solve(puzzle_input: str) -> tuple[any, any]:
    inp = puzzle_input.splitlines()
    towels = [t.strip() for t in inp[0].split(",")]
    designs = inp[2:]
    part_one = part1(towels, designs)
    part_two = part2(towels, designs)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=True)
