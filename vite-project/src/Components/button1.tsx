import 'bootstrap/dist/css/bootstrap.min.css';
import { Button } from "react-bootstrap";

interface Button1Props {
    state: boolean;
    textBuy: string;
    textBought: string;
    fun: React.Dispatch<React.SetStateAction<boolean>>;
  }

  

  
export function Button1({ state, fun, textBuy, textBought }: Button1Props) {
    const variant = state ? 'success' : 'primary';
    const text = state ? textBought : textBuy;
    return (
        <Button variant={variant} onClick={() => fun(!state)}>{text}</Button>
    );
}