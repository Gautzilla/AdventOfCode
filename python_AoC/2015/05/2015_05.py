from template_creator import read_input
from pandas import Timestamp, Timedelta

def has_three_vowels(s: str) -> bool:
	return sum(1 for c in s if c in "aeiou") >= 3

def has_repeated_letter(s: str, nb_letters_between: int) -> bool:
	gap = nb_letters_between+1
	return any(s[i] == s[i-gap] for i in range(gap,len(s)))

def has_forbidden_str(s: str) -> bool:
	return any(f in s for f in ("ab", "cd", "pq", "xy"))

def has_repeated_pair(s: str) -> bool:
	for index in range(len(s)-2):
		chunk = s[index:index+2]
		if chunk in s[index+2:]:
			return True
	return False

def is_nice_part1(s: str) -> bool:
	return has_three_vowels(s) and has_repeated_letter(s, nb_letters_between=0) and not has_forbidden_str(s)

def is_nice_part2(s: str) -> bool:
	return has_repeated_letter(s, nb_letters_between=1) and has_repeated_pair(s)

def part1(puzzle_input: list[str]) -> any:
	return sum(1 for s in puzzle_input if is_nice_part1(s))

def part2(puzzle_input: list[str]) -> any:
	return sum(1 for s in puzzle_input if is_nice_part2(s))

def solve(puzzle_input: str) -> tuple[any, any]:
	puzzle_input = puzzle_input.split("\n")
	part_one = part1(puzzle_input)
	part_two = part2(puzzle_input)

	return part_one, part_two

if __name__ == "__main__":
	i = read_input(example = False)

	t_start = Timestamp.now()
	p1, p2 = solve(puzzle_input=i)
	t_stop = Timestamp.now()
	t = Timedelta(t_stop - t_start).total_seconds()

	print(f"{'Part one':<20}{p1:>10}")
	print(f"{'Part two':<20}{p2:>10}")
	print(f"\nSolved in {t} second.")