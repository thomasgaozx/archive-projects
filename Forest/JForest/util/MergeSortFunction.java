package forest.util;

import java.util.*;

public class MergeSortFunction implements SortFunction {

	public <T> void sort(List<T> list, Comparator<T> comparator) throws NullPointerException {
		if (list==null) {
			throw new NullPointerException();
		}

		new MergeSort<T>(list,comparator).sort();
	}
}