import { useEffect, useState } from 'react';
import Card from 'react-bootstrap/Card';
import { productApiGetItem } from '../Api/Api';
import { CartButton } from './CartButton';
import { useNavigate } from "react-router-dom";



interface CardProps {
    id: number;
    name: string;
    description: string;
    price: string;
    image: string;
}

export interface ItemInUser {
    Id: number;
    UserId: number;
    isInCart: boolean;
    isFavourite: boolean;
    itemInCartNumber: number;
    itemId: number;
}

function Card1({ id, name, description, image, price }: CardProps) {

    const [data, setData] = useState<ItemInUser>();
    const s = useNavigate();

    useEffect(() => {
        productApiGetItem("getInfoById", id).then(res => setData(res.data));
    }, [data]);

    const handleBuyClick = async (itemId: number) => {
        try {
            await productApiGetItem("getNumInCartById", itemId);
        } catch (error) {
            console.error("Error fetching1 numInCart:", error);
            window.location.href = "/login";
        }
    };

    const handleAddClick = async (itemId: number) => {
        try {
            await productApiGetItem("addNumInCartById", itemId);

        } catch (error) {
            console.error("Error fetching2 numInCart:", error);
            s("/login");
        }
    };

    const handleRemClick = async (itemId: number) => {
        try {
            await productApiGetItem("minusNumInCartById", itemId);

        } catch (error) {
            console.error("Error fetching3 numInCart:", error);
            s("/login");
        }
    };


    return (
        <Card style={{ width: '18rem' }}>
            <Card.Img variant="top" src={image} />
            <Card.Body>
                <Card.Title>{name}</Card.Title>
                <Card.Text>
                    {description}
                </Card.Text>
                <CartButton
                    state={data?.isInCart}
                    rand={false}
                    funBuy={() => handleBuyClick(id)}
                    funAdd={() => handleAddClick(id)}
                    funRem={() => handleRemClick(id)}
                    //fun={() => setData()}
                    textBuy={price}
                    textBought={'В корзине:' + data?.itemInCartNumber}
                    inCartNum={data?.itemInCartNumber}
                    isFavourite={false}
                    id={id} />
            </Card.Body>
        </Card>
    );
}

export default Card1;