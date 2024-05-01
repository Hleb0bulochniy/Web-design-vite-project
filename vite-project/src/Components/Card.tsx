import Card from 'react-bootstrap/Card';

interface CardProps{
    name: string;
    description: string;
    price: string;
    image: string;
    button: JSX.Element;
}

function Card1({ name, description, image, button }: CardProps) {
    return (
        <Card style={{ width: '18rem' }}>
            <Card.Img variant="top" src={image} />
            <Card.Body>
                <Card.Title>{name}</Card.Title>
                <Card.Text>
                    {description}
                </Card.Text>
                {button}
            </Card.Body>
        </Card>
    );
}

export default Card1;