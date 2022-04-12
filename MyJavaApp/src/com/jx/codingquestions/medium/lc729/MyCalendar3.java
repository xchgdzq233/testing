package com.jx.codingquestions.medium.lc729;

import java.util.ArrayList;
import java.util.List;

public class MyCalendar3 implements MyCalendar {

    private final List<Integer> myStarts;
    private final List<Integer> myEnds;

    public MyCalendar3() {

        myStarts = new ArrayList<>();
        myEnds = new ArrayList<>();
    }

    @Override
    public boolean book(final int start, final int end) {

        if (!isValidEvent(start, end)) {
            return false;
        }

        final int indexToInsert = myStarts.size() == 0 ? 0 : findIndexToInsert(start, end, 0, myStarts.size() - 1);
        if (indexToInsert == -1) {
            return false;
        }

        myStarts.add(indexToInsert, start);
        myEnds.add(indexToInsert, end);

        return true;
    }

    @Override
    public void printQueue() {

        for (int i = 0; i < myStarts.size(); i++) {
            System.out.printf("(%s, %s), ", myStarts.get(i), myEnds.get(i));
        }
        System.out.println();
    }

    private boolean isValidEvent(final int start, final int end) {

        return start < end && start >= 0;
    }

    private int findIndexToInsert(final int start, final int end, final int prevLeftIndex, final int prevRightIndex) {

        final int curStart = myStarts.get(prevLeftIndex);
        final int curEnd = myEnds.get(prevRightIndex);

        if (end <= curStart) {
            return prevLeftIndex;
        } else if (start >= curEnd) {
            return prevRightIndex;
        } else {
            final int midIndex = (prevLeftIndex + prevRightIndex) / 2;
            if (midIndex >= prevRightIndex) {
                return -1;
            }

            final int midStart = myStarts.get(midIndex);
            final int midEnd = myEnds.get(midIndex);

            if (end <= midStart) {
                return findIndexToInsert(start, end, prevLeftIndex, midIndex);
            } else if (start >= midEnd) {
                return findIndexToInsert(start, end, midIndex + 1, prevRightIndex);
            } else {
                return -1;
            }
        }
    }
}
