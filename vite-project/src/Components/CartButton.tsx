import 'bootstrap/dist/css/bootstrap.min.css';
import { Button } from "react-bootstrap";
import React from 'react';
import { useNavigate } from 'react-router-dom';

interface Button1Props {
    state?: boolean;
    rand: boolean;
    textBuy: string;
    textBought: string;
    funBuy: React.Dispatch<React.SetStateAction<boolean>>;
    funAdd: React.Dispatch<React.SetStateAction<boolean>>;
    funRem: React.Dispatch<React.SetStateAction<boolean>>;
    inCartNum?: number;
    isFavourite: boolean;
    id: number;
}



export function CartButton({ state, funAdd, funRem, funBuy, textBuy, textBought, inCartNum, isFavourite, id , rand}: Button1Props) {
    const variant = state ? 'success' : 'primary';
    const text = state ? textBought : textBuy;
    const s = useNavigate();


    if (state) {
        return (
            <>
                <Button variant={variant} onClick={() => { s("/cart"); }}>{textBought}</Button>
                <Button variant="primary" onClick={() => {funAdd(!rand);}}>
                    +
                </Button>
                <Button variant="danger" onClick={() => {funRem(!rand);}}>
                    -
                </Button>
            </>
        );
    }
    if (!state) {
        return (
            <Button variant={variant} onClick={() => {funBuy(!rand)}}>{text}</Button>
        );
    }
}
