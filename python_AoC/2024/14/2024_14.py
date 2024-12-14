from template_creator import read_input
from pandas import Timestamp, Timedelta
import re
import os

def move_robots(robots, gx, gy, nb_times):
	for robot in robots:
		x,y,vx,vy = robot
		robot[0] = (x + vx * nb_times + gx * (abs(x) // gx + 1)) % gx
		robot[1] = (y + vy * nb_times + gy * (abs(y) // gy + 1)) % gy
	return robots

def print_robots(robots, gx, gy):
	robots = [(x,y) for x,y,*_ in robots]
	for y in range(gy):
		line = "".join("." if (x,y) not in robots else "*" for x in range(gx))
		print(line)

def part1(robots, gx, gy) -> any:
	robots = [(x,y) for x,y,*_ in move_robots(robots, gx, gy, 100)]
	q1 = sum(1 for x,y in robots if x < gx//2 and y < gy//2)
	q2 = sum(1 for x,y in robots if x > gx//2 and y < gy//2)
	q3 = sum(1 for x,y in robots if x < gx//2 and y > gy//2)
	q4 = sum(1 for x,y in robots if x > gx//2 and y > gy//2)
	return q1*q2*q3*q4

def part2(robots, gx, gy) -> any:
	nb_seconds = 0
	while True:
		nb_seconds+=1
		robots = move_robots(robots, gx, gy, 1)
		if has_block(robots):
			print("\n" * 100)
			print_robots(robots, gx, gy)
			print(f"\n{nb_seconds}\n")
			break
	return nb_seconds

def has_block(robots):
	neighbours = ((-1,0),(-1,-1),(0,-1),(1,-1),(1,0),(1,1),(0,1),(-1,1))
	robots = [(x,y) for x,y,*_ in robots]
	return any(all((x+nx, y+ny) in robots for nx,ny in neighbours) for x,y in robots[::8])


def solve(puzzle_input: str, example: bool) -> tuple[any, any]:
	robots = [list(map(int,robot)) for robot in (re.search(r"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)", line).groups() for line in puzzle_input.splitlines())]
	gx, gy = (11,7) if example else (101,103)
	part_one = part1(robots, gx, gy)
	robots = [list(map(int,robot)) for robot in (re.search(r"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)", line).groups() for line in puzzle_input.splitlines())]
	part_two = part2(robots, gx, gy) if not example else ""

	return part_one, part_two

if __name__ == "__main__":
	example = False
	i = read_input(example)

	t_start = Timestamp.now()
	p1, p2 = solve(puzzle_input=i, example=example)
	t_stop = Timestamp.now()
	t = Timedelta(t_stop - t_start).total_seconds()

	print(f"{'Part one':<20}{p1:>10}")
	print(f"{'Part two':<20}{p2:>10}")

	print(f"\nSolved in {t} second{'s' if t >= 2 else ''}.")