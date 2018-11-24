package forest.util;

import java.util.Queue;
import java.util.Stack;
import java.util.LinkedList;
import java.util.List;
import java.util.Comparator;

// by default sort in ascending order
// the reason its contained in the class is that it could avoid passing list every single time while doing a recursive call
class MergeSort<T> {

	private List<T> list;
	private Comparator<T> comparator;

	private void sort(int start, int end) {
		if (start<end) {

			// set a cut-off point
			int cut = (start+end)/2;

			// recursive calls
			sort(start, cut);
			sort(cut+1, end);
			
			// populate queues for first and second half of the list
			Queue<T> first = new LinkedList<T>();
			Queue<T> second = new LinkedList<T>();

			for (int i=start; i<cut; ++i) {
				first.add(list.get(i));
			}

			for (int j=cut+1; j<end; ++j) {
				second.add(list.get(j));
			}

			// compare and insert
			int insertIndex=0;
			T firstHold = first.remove(), secondHold = second.remove();
			while (!(first.isEmpty()||second.isEmpty())) {

				if (comparator.compare(firstHold,secondHold)>0) {
					list.set(insertIndex++,secondHold);
					secondHold=second.remove();
				} else {
					list.set(insertIndex++,firstHold);
					firstHold=first.remove();
				}

			}
			
			// finish off the remaining
			if (first.isEmpty()) { // second is not empty
				while (!second.isEmpty()) {
					list.set(insertIndex++,second.remove());
				}
			} else {
				while (!first.isEmpty()) {
					list.set(insertIndex++,second.remove());
				}
			}
		}

	}

	public void sort() {
		sort(0, list.size()-1);
	}

	public MergeSort(List<T> list, Comparator<T> comparator) {
		this.list = list;
		this.comparator = comparator;
	}

}