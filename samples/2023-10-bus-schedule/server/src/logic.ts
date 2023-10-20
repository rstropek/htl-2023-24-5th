export interface BusPlan {
    lines: {
        [key: string]: Line;
    };
    ticketPrices: TicketPrices;
    rebates: Rebates;
}

export interface Line {
    stops: Stop[];
    schedule: Schedule;
}

export interface Stop {
    name: string;
    travelTime: number;
}

export interface Schedule {
    start: number;
    last: number;
    every: number;
}

export interface TicketPrices {
    mini: number; // max. 4 stops including start and end
    midi: number; // max. 8 stops including start and end
    maxi: number; // unlimited stops
}

export interface NextBusResponse {
    line: string;
    stop: string;
    departure: number;
}

export interface Rebates {
    student: number;
    senior: number;
}

export interface TicketParameters {
    line: string;
    from: string;
    to: string;
    rebate: ('student' | 'senior' | undefined | null);
}

export interface TicketResponse {
    ticket: 'mini' | 'midi' | 'maxi';
    price: number;
}

export function nextDeparture(plan: BusPlan, line: string, from: string, start: number): (number | null) {
    // Verify that line exists
    const bus = plan.lines[line];
    if (!bus) {
        throw new Error('Invalid line');
    }

    // Verify that from exists in given line
    const stop = bus.stops.find((stop) => stop.name === from);
    if (!stop) {
        throw new Error('Invalid stop');
    }

    // Verify that hour and minute are valid
    if (start < 0 || start > 23) {
        throw new Error('Invalid hour or minute');
    }

    // Find next departure
    let nextLeave = bus.schedule.start;
    while (start > (nextLeave + stop.travelTime/60)) {
        nextLeave += bus.schedule.every / 60;
    }
    
    if (nextLeave > bus.schedule.last) { return null; }
    
    return nextLeave + stop.travelTime/60;
}

export function buildBusPlanResponse(line: string, bus: Line, from: string, next: number): NextBusResponse[] {
    let departingStop: Stop | undefined;
    const response: NextBusResponse[] = [];
    for (const stop of bus.stops) {
        if (!departingStop && stop.name === from) {
            departingStop = stop;
        }

        if (departingStop) {
            response.push({
                line,
                stop: stop.name,
                departure: next + (stop.travelTime - departingStop.travelTime) / 60,
            });
        }
    }

    return response;
}

export function getSortedStopNames(plan: BusPlan) : string[] {
    const stopNames: string[] = [];
    for (const line in plan.lines) {
        for (const stop of plan.lines[line].stops) {
            if (!stopNames.includes(stop.name)) {
                stopNames.push(stop.name);
            }
        }
    }

    stopNames.sort();

    return stopNames;
}

export function getSortedLineNames(plan: BusPlan): string[] {
    const lineNames: string[] = [];
    for (const line in plan.lines) {
        lineNames.push(line);
    }

    lineNames.sort();

    return lineNames;
}

export function getNumberOfStationsBetween(plan: BusPlan, line: string, from: string, to: string): number {
    const bus = plan.lines[line];
    if (!bus) {
        throw new Error('Invalid line');
    }

    const fromStop = bus.stops.find((stop) => stop.name === from);
    if (!fromStop) {
        throw new Error('Invalid stop');
    }

    const toStop = bus.stops.find((stop) => stop.name === to);
    if (!toStop) {
        throw new Error('Invalid stop');
    }

    const fromIndex = bus.stops.indexOf(fromStop);
    const toIndex = bus.stops.indexOf(toStop);

    return Math.abs(toIndex - fromIndex + 1);
}

export function getTicketPrice(numberOfStops: number, ticketPrices: TicketPrices, rebates: Rebates, rebate: ('student' | 'senior' | undefined | null) ) : TicketResponse {
    const rebatePercentage = (rebate ? (1 - rebates[rebate]) : 1);
    if (numberOfStops <= 4) {
        return {
            ticket: 'mini',
            price: ticketPrices.mini * rebatePercentage,
        };
    } else if (numberOfStops <= 8) {
        return {
            ticket: 'midi',
            price: ticketPrices.midi * rebatePercentage,
        };
    } else {
        return {
            ticket: 'maxi',
            price: ticketPrices.maxi * rebatePercentage,
        };
    }
}
