package forest;

import java.util.*;

class NormalComparator<T extends Comparable<T>> implements Comparator<T> {
	public int compare(T a, T b) {
		return a.compareTo(b);
	}
}