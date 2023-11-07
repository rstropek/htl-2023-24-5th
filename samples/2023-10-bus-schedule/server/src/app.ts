import express from 'express';
import { readFileSync } from 'fs';
import { resolve } from 'path';
import { BusPlan, NextBusResponse, Stop, TicketResponse, buildBusPlanResponse, getNumberOfStationsBetween, getSortedLineNames, getSortedStopNames, getTicketPrice, nextDeparture } from './logic';

const app = express();
const PORT = 3000;

app.use(express.json());
app.get('/api/bus/:line', (req: express.Request, res: express.Response) => {
    try {
        // Load data
        const data: BusPlan = JSON.parse(readFileSync(resolve(__dirname, '../plan.json'), 'utf-8'));

        // Extract parameters
        const line = req.params.line;
        const from = req.query.from as string;
        const start = parseFloat(req.query.start as string);

        if (!from || isNaN(start)) {
            return res.status(400).json({ error: 'Invalid hour or minute' });
        }

        let next: (number | null);
        try {
            next = nextDeparture(data, line, from, start);
        } catch (error) {
            let message: string = '';
            if (error instanceof Error) {
                message = error.message;
            }
            
            return res.status(400).json(message);
        }

        if (next === null) {
            // Return not found
            return res.status(404).json({ error: 'No more departures today' });
        }

        const response = buildBusPlanResponse(line, data.lines[line], from, next);

        return res.json(response);
    } catch (error) {
        res.status(500).json({ error: 'Server Error' });
    }
});

app.get('/api/stops', (req: express.Request, res: express.Response) => {
    try {
        const data: BusPlan = JSON.parse(readFileSync(resolve(__dirname, '../plan.json'), 'utf-8'));
        return res.json(getSortedStopNames(data));
    } catch (error) {
        res.status(500).json({ error: 'Server Error' });
    }
});

app.get('/api/lines', (req: express.Request, res: express.Response) => {
    try {
        const data: BusPlan = JSON.parse(readFileSync(resolve(__dirname, '../plan.json'), 'utf-8'));
        return res.json(getSortedLineNames(data));
    } catch (error) {
        res.status(500).json({ error: 'Server Error' });
    }
});

app.post('/api/price', (req: express.Request, res: express.Response) => {
    try {
        const data: BusPlan = JSON.parse(readFileSync(resolve(__dirname, '../plan.json'), 'utf-8'));

        const { line, from, to, rebate } = req.body;
        if (!line || !from || !to) {
            return res.status(400).json({ error: 'Invalid parameters' });
        }

        let price: TicketResponse;
        try {
            const numberOfStations = getNumberOfStationsBetween(data, line, from, to);
            price = getTicketPrice(numberOfStations, data.ticketPrices, data.rebates, rebate);
        } catch (error) {
            let message: string = '';
            if (error instanceof Error) {
                message = error.message;
            }
            
            return res.status(400).json(message);
        }

        return res.json(price);
    } catch (error) {
        res.status(500).json({ error: 'Server Error' });
    }
});

app.listen(PORT, () => {
    console.log(`Server is running on port ${PORT}.`);
});
