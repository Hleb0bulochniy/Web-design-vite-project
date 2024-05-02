import axios from "axios";

export const apiIns = axios.create({
    baseURL: "https://localhost:7287/",
    headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
    },
});

export function productApi(d: string) {
    return apiIns.get(d);
}

export function productApiGetItem(d: string, id: number) {
    return id ? apiIns.get(`${d}/${id}`) : apiIns.get(d);
}