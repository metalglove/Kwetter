import http from 'k6/http';
import { check, sleep } from 'k6';

export const GATEWAY_API_URL = "http://neuralm.net/api";

export let options = {
    stages: [
        { duration: '2m', target: 100 }, // simulate ramp-up of traffic from 1 to 100 users over 2 minutes.
        { duration: '4m', target: 100 }, // stay at 100 users for 4 minutes
        { duration: '2m', target: 0 },   // ramp-down to 0 users
    ],
    thresholds: {
        http_req_duration: ['avg < 400', 'p(99) < 2000'],
        checks: ['rate>0.9']
    }
};

function verifyUserNameUniqueness(username) {
    const url = `${GATEWAY_API_URL}/Authorization/VerifyUserNameUniqueness`;
    const payload = JSON.stringify({ "UserName": username });
    const params = {
        headers: {
            'Content-Type': 'application/json'
        },
    };
    const res = http.post(url, payload, params);
    check(res, {
        'status is 200': (r) => r.status == 200,
    });
}

export default function () {
    verifyUserNameUniqueness("xxmegamanxx");
    sleep(1);
}
