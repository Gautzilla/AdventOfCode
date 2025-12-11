from __future__ import annotations

import math
from typing import Iterable

from template_creator import run_puzzle

class Circuit:
    def __init__(self, boxes: Iterable[Box]) -> None:
        self.boxes = {*boxes}

    def __contains__(self, box: Box) -> bool:
        return box in self.boxes

    @property
    def size(self) -> int:
        return sum(1 for _ in self.boxes)

    def add(self, box) -> None:
        self.boxes.add(box)

    @classmethod
    def merge(cls, c1: Circuit, c2: Circuit) -> Circuit:
        return cls(c1.boxes.union(c2.boxes))

class Box:
    def __init__(self, coords: str) -> None:
        self.coords = tuple(map(int, coords.split(",")))
        self.circuit = None

    def distance(self, other: Box) -> float:
        return math.sqrt(sum((sc-oc)**2 for sc, oc in zip(self.coords, other.coords)))

    def __repr__(self) -> str:
        return ",".join(map(str,self.coords))

def parse_boxes(puzzle_input: str) -> list[Box]:
    return [Box(coords=line) for line in puzzle_input.splitlines()]

def parse_distances(boxes: list[Box]) -> dict[tuple[Box,Box], float]:
    output = {}
    
    for idx,box1 in enumerate(boxes):
        for box2 in boxes[idx+1:]:
            output |= {(box1, box2): box1.distance(box2)}
    return output

def link(box1: Box, box2: Box, circuits: set[Circuit]) -> None:
    c1 = next((circuit for circuit in circuits if box1 in circuit), None)
    c2 = next((circuit for circuit in circuits if box2 in circuit), None)

    if c1 and c2:
        if c1 is c2:
            return
        circuits.remove(c1)
        circuits.remove(c2)
        circuits.add(Circuit.merge(c1, c2))
    if c1:
        c1.add(box2)
        return
    if c2:
        c2.add(box1)
        return
    circuits.add(Circuit((box1, box2)))

def part1(boxes: list[Box]) -> int:
    distances = list(sorted(parse_distances(boxes).items(), key = lambda couple: couple[1]))[:1000]
    circuits = set()
    while distances:
        (box1,box2),_ = distances.pop(0)
        link(box1,box2,circuits)
    c1,c2,c3 = map(lambda c: c.size,list(sorted(circuits, key= lambda c: c.size))[-3:])
    return c1*c2*c3

def part2(puzzle_input: str) -> any:
    return ""


def solve(puzzle_input: str) -> tuple[any, any]:
    boxes = parse_boxes(puzzle_input)
    part_one = part1(boxes)
    part_two = part2(puzzle_input)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=False)
