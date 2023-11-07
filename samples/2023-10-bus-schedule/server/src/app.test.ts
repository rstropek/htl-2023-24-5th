import { BusPlan, buildBusPlanResponse, getNumberOfStationsBetween, getSortedLineNames, getSortedStopNames, getTicketPrice, nextDeparture } from './logic';
import { toMatchCloseTo } from 'jest-matcher-deep-close-to';
expect.extend({ toMatchCloseTo });

const plan: BusPlan = {
  lines: {
    "1": {
      stops: [
        { name: "A", travelTime: 0 },
        { name: "B", travelTime: 10 },
        { name: "C", travelTime: 20 },
      ],
      schedule: {
        start: 6,
        every: 30,
        last: 22,
      },
    },
    190: {
        stops: [
            { name: "Main Square", "travelTime": 0 },
            { name: "Harbor", "travelTime": 52 },
            { name: "Airport", "travelTime": 60 }
        ],
        schedule: {
            start: 7,
            last: 21,
            every: 30
        }
    }
  },
  ticketPrices: {
    mini: 1, midi: 2, maxi: 3,
  },
  rebates: { student: 0.25, senior: 0.5 },
};

describe('next departure', () => {
  test('first is next', () => {
    expect(nextDeparture(plan, '1', 'A', 5)).toBe(6);
    expect(nextDeparture(plan, '1', 'B', 5)).toBe(6 + 10 / 60);
  });

  test('next', () => {
    expect(nextDeparture(plan, '1', 'A', 6.25)).toBe(6.5);
    expect(nextDeparture(plan, '1', 'B', 6.25)).toBe(6.5 + 10 / 60);
  });

  test('next 190', () => {
    expect(nextDeparture(plan, '190', 'Harbor', 22)).toBeNull();
  });

  test('last is next', () => {
    expect(nextDeparture(plan, '1', 'A', 21.75)).toBe(22);
    expect(nextDeparture(plan, '1', 'B', 21.75)).toBe(22 + 10 / 60);
  });

  test('no next', () => {
    expect(nextDeparture(plan, '1', 'A', 22.75)).toBeNull();
    expect(nextDeparture(plan, '1', 'B', 22.75)).toBeNull();
  });
});

describe('build bus plan', () => {
  test('from start to end', () => {
    expect(buildBusPlanResponse('1', plan.lines['1'], 'A', 6)).toMatchCloseTo([
      { line: '1', stop: 'A', departure: 6 },
      { line: '1', stop: 'B', departure: 6 + 10 / 60 },
      { line: '1', stop: 'C', departure: 6 + 20 / 60 },
    ]);
  });
  test('starting in the middle', () => {
    expect(buildBusPlanResponse('1', plan.lines['1'], 'B', 6 + 10 / 60)).toMatchCloseTo([
      { line: '1', stop: 'B', departure: 6 + 10 / 60 },
      { line: '1', stop: 'C', departure: 6 + 20 / 60 },
    ]);
  });
});

describe('stops', () => {
  test('get sorted list', () => {
    expect(getSortedStopNames(plan)).toEqual([
      'A', 'Airport', 'B', 'C', 'Harbor', 'Main Square',
    ]);
  });

  test('get sorted lines', () => {
    expect(getSortedLineNames(plan)).toEqual([
      '1', '190',
    ]);
  });

  test('number of stops between', () => {
    expect(getNumberOfStationsBetween(plan, '1', 'A', 'C')).toBe(3);
    expect(getNumberOfStationsBetween(plan, '1', 'A', 'B')).toBe(2);
    expect(getNumberOfStationsBetween(plan, '1', 'A', 'A')).toBe(1);
  });
});

describe('ticket prices', () => {
  test('mini', () => {
    expect(getTicketPrice(2, plan.ticketPrices, plan.rebates, undefined)).toEqual({ ticket: 'mini', price: 1 });
    expect(getTicketPrice(2, plan.ticketPrices, plan.rebates, 'student')).toEqual({ ticket: 'mini', price: 1 * 0.75 });
    expect(getTicketPrice(2, plan.ticketPrices, plan.rebates, 'senior')).toEqual({ ticket: 'mini', price: 0.5 });
  });
  test('midi', () => {
    expect(getTicketPrice(5, plan.ticketPrices, plan.rebates, undefined)).toEqual({ ticket: 'midi', price: 2 });
    expect(getTicketPrice(5, plan.ticketPrices, plan.rebates, 'student')).toEqual({ ticket: 'midi', price: 1.5 });
    expect(getTicketPrice(5, plan.ticketPrices, plan.rebates, 'senior')).toEqual({ ticket: 'midi', price: 1 });
  });
  test('maxi', () => {
    expect(getTicketPrice(10, plan.ticketPrices, plan.rebates, undefined)).toEqual({ ticket: 'maxi', price: 3 });
    expect(getTicketPrice(10, plan.ticketPrices, plan.rebates, 'student')).toEqual({ ticket: 'maxi', price: 2.25 });
    expect(getTicketPrice(10, plan.ticketPrices, plan.rebates, 'senior')).toEqual({ ticket: 'maxi', price: 1.5 });
  });
});

