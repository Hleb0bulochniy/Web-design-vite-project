import { Button } from "react-bootstrap";
import { useAppDispatch } from "../Redux/Hooks";
import { decrement, increment } from "../Redux/CounterSlice";

export function Button2(){
    const dispatch = useAppDispatch();
    return(
        <>
            <Button variant = "primary" onClick={()=> dispatch(increment())}>
            +
            </Button>
            <Button variant = "primary" onClick={()=> dispatch(decrement())}>
            -
            </Button>
        </>

    )
}