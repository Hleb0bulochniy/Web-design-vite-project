import { ChangeSum } from "./SumSlice";



export const SumUpd = () => async (dispatch: any) => {
    try {
        const response = await fetch('https://localhost:7287/Sum', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
            },
        });
        const result = await response.json();
        if (response.ok) {
            dispatch(ChangeSum(result.sum));
        } else {
        }
    } catch (error) {
        
    }
};