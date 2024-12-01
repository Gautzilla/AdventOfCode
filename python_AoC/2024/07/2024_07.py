from template_creator import read_input
from pandas import Timestamp, Timedelta

def solve() -> tuple[int, int]:
	part_one = 0
	part_two = 0

	return part_one, part_two

if __name__ == "__main__":
	i = read_input(example = True)

	t_start = Timestamp.now()
	p1, p2 = solve()
	t_stop = Timestamp.now()
	t = Timedelta(t_stop - t_start).total_seconds()

	print(f"{'Part one':<20}{p1:>10}")
	print(f"{'Part two':<20}{p2:>10}")
	print(f"\nSolved in {t} second.")