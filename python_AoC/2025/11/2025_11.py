from template_creator import run_puzzle


def parse_input(puzzle_input: str) -> dict[str, list[str]]:
    devices = {}
    for line in puzzle_input.splitlines():
        device, links = line.split(":", maxsplit=1)
        devices[device] = [link.strip() for link in links.split()]
    return devices

def DFS(device: str, devices: dict[str, list[str]], cache: dict[str, int]) -> int:
    if device == "out":
        return 1
    if device in cache:
        return cache[device]
    paths=0
    for link in devices[device]:
        paths += DFS(link, devices, cache)
    cache[device] = paths
    return paths

def DFS_p2(device: str, devices: dict[str,list[str]], dac_and_fft_visited: int, cache: dict[tuple[str,int], int]):
    if device == "out":
        return 1 if dac_and_fft_visited == (1|2) else 0
    key = (device, dac_and_fft_visited)
    if key in cache:
        return cache[key]
    if device == "dac":
        dac_and_fft_visited |= 1
    if device == "fft":
        dac_and_fft_visited |= 2
    paths=0
    for link in devices[device]:
        paths += DFS_p2(link, devices, dac_and_fft_visited, cache)
    cache[key] = paths
    return paths


def part1(devices: dict[str, list[str]]) -> any:
    return DFS("you", devices, {})

def part2(devices: dict[str, list[str]]) -> any:
    return DFS_p2("svr", devices, 0, {})


def solve(puzzle_input: str) -> tuple[any, any]:
    devices = parse_input(puzzle_input)
    part_one = 1 #part1(devices)
    part_two = part2(devices)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=False)
