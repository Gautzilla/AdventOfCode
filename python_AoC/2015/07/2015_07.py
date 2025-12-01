from template_creator import run_puzzle

OPERATIONS = {
    "AND": lambda x, y: x & y,
    "OR": lambda x, y: x | y,
    "RSHIFT": lambda x, y: x >> y,
    "LSHIFT": lambda x, y: x << y,
    "NOT": lambda x: ~x,
}


def get_value(wire, wires):
    if wire.isdigit():
        return int(wire)
    if wire in wires:
        return wires[wire]
    return None


def solve_puzzle(operations, wires) -> any:
    while operations and "a" not in wires:
        operation = operations.pop(0)
        left, right = operation
        left_parts = left.split()
        if len(left_parts) == 1:
            if (v := get_value(left_parts[0], wires)) is not None:
                wires[right] = v
                continue
        if len(left_parts) == 2 and left_parts[0] == "NOT":
            if (v := get_value(left_parts[1], wires)) is not None:
                wires[right] = (~v) & 65535
                continue
        if len(left_parts) == 3:
            x, op, y = left_parts
            x, y = (get_value(v, wires) for v in (x, y))
            if all(v is not None for v in (x, y)):
                wires[right] = OPERATIONS[op](x, y) % 65535
                continue
        operations.append((left, right))

    return wires["a"], wires


def solve(puzzle_input: str) -> tuple[any, any]:
    operations = [
        tuple(part.strip() for part in operation.split("->"))
        for operation in puzzle_input.splitlines()
    ]
    wires = {}
    part_one, wires = solve_puzzle(operations, wires)
    wires.clear()
    wires["b"] = part_one
    operations = [
        tuple(part.strip() for part in operation.split("->"))
        for operation in puzzle_input.splitlines()
    ]
    operations = [o for o in operations if o[-1] != "b"]
    part_two, _ = solve_puzzle(operations, wires)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=True)
