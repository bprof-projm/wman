import "./card.styles.css";

const MyCard = (props) => {
    
    const { username, email, firstname, lastname, phoneNumber } = props.object;
    let src = "";
    src = `https://eu.ui-avatars.com/api/?name=${firstname} ${lastname}`;
    
    return (
        <div className="card-container" onClick={() => props.func(props.object)}>
            <div className="card-top">
                <div className="card-avatar">
                    <img src={src} alt={src}></img>
                </div>
                <div className="card-data">
                    <h3>{firstname} {lastname}</h3>
                    <h4>{username}</h4>                    
                    <h4>{phoneNumber}</h4>
                    <h4>{email}</h4>                    
                </div>
            </div>
        </div>
    );
};

export default MyCard;