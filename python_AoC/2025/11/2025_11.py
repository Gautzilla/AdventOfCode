from template_creator import run_puzzle


def parse_input(puzzle_input: str) -> dict[str, list[str]]:
    devices = {}
    for line in puzzle_input.splitlines():
        device, links = line.split(":", maxsplit=1)
        devices[device] = [link.strip() for link in links.split()]
    return devices

nb_paths_to_out = {}

def DFS(device: str, devices: dict[str, list[str]]) -> int:
    if device == "out":
        return 1
    if device in nb_paths_to_out:
        return nb_paths_to_out[device]+1
    paths=0
    for link in devices[device]:
        paths += DFS(link, devices)
    nb_paths_to_out["device"] = paths
    return paths

def part1(devices: dict[str, list[str]]) -> any:
    return DFS("you", devices)

def part2(puzzle_input: str) -> any:
    return ""


def solve(puzzle_input: str) -> tuple[any, any]:
    devices = parse_input(puzzle_input)
    part_one = part1(devices)
    part_two = part2(puzzle_input)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=False)
