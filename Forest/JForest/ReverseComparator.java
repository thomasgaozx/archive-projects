package forest;

import java.util.*;

class ReverseComparator<T extends Comparable<T>> implements Comparator<T> {
	public int compare(T a, T b) {
		return b.compareTo(a);
	}
}