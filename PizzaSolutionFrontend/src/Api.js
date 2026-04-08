const BASE_URL = 'http://localhost:9009';

// GET requests
export async function getWelcome() {
    const response = await fetch(`${BASE_URL}/api/welcome`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        },
    });

    if (!response.ok)
        throw new Error(`Welcome GET failed: ${response.statusText}`);

    return await response.text();
}

export async function getMenu() {
    const response = await fetch(`${BASE_URL}/api/menu`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        },
    }); //should give it the current time since GetMenu needs that

    if (!response.ok)
        throw new Error(`Menu GET failed: ${response.statusText}`);

    return await response.json();
}

export async function makeOrder(newOrder) {
    const response = await fetch(`${BASE_URL}/api/order`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ order: newOrder }),
    });

    if (!response.ok)
        throw new Error(`Ordering POST failed: ${response.statusText}`);

    return await response.json();
}