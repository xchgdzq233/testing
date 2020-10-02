package com.jx.leetcode.medium.lc729;

import static org.junit.Assert.assertEquals;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;
import org.junit.Before;
import org.junit.Test;

public class MyCalendarTest {

    private List<MyCalendar> calendars;

    @Before
    public void setup() {

        calendars = new ArrayList<>();
        calendars.add(new MyCalendar1());
        calendars.add(new MyCalendar2());
        calendars.add(new MyCalendar3());
    }

    private void assertAll(final int start, final int end, final boolean expectedResult) {

        for (int i = 0; i < calendars.size(); i++) {
            if(i == 2){
                System.out.printf("MyCalendar%s inserting (%s, %s) and expecting %s. ", i + 1, start, end, expectedResult);
            }
            final MyCalendar curCalendar = calendars.get(i);
            assertEquals(curCalendar.book(start, end), expectedResult);
            if (i == 2) {
                System.out.printf("MyCalendar%s test passed! Insert (%s, %s): %s. Current Queue: ",
                                  i + 1, start, end, expectedResult);
                curCalendar.printQueue();
            }
        }
    }

    @Test
    public void test1() {

        assertAll(10, 20, true);
        assertAll(15, 25, false);
        assertAll(20, 30, true);
    }

    @Test
    public void test2() {

        assertAll(47, 50, true);
        assertAll(33, 41, true);
        assertAll(39, 45, false);
        assertAll(33, 42, false);
        assertAll(25, 32, true);
        assertAll(26, 35, false);
        assertAll(19, 25, true);
        assertAll(3, 8, true);
        assertAll(8, 13, true);
        assertAll(18, 27, false);
    }

    @Test
    public void test() {

        List<Integer> list = new ArrayList<>();
        list.add(0, 1);
        assertEquals(Optional.ofNullable(list.get(0)), Optional.of(1));
        list.add(1, 2);
        assertEquals(Optional.ofNullable(list.get(1)), Optional.of(2));
    }
}