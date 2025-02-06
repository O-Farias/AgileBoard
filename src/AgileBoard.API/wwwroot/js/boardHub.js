class BoardHubClient {
    constructor() {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl("/hubs/board")
            .withAutomaticReconnect()
            .build();

        this.setupEventHandlers();
        this.startConnection();
    }

    async startConnection() {
        try {
            await this.connection.start();
            console.log("Conectado ao BoardHub");
        } catch (err) {
            console.error("Erro ao conectar:", err);
            setTimeout(() => this.startConnection(), 5000);
        }
    }

    setupEventHandlers() {
        this.connection.on("CardCreated", (card) => {
            console.log("Card criado:", card);
            // Atualizar UI - exemplo:
            this.addCardToList(card);
        });

        this.connection.on("CardUpdated", (card) => {
            console.log("Card atualizado:", card);
            // Atualizar UI
            this.updateCardInList(card);
        });

        this.connection.on("CardMoved", (cardId, listId, position) => {
            console.log("Card movido:", { cardId, listId, position });
            // Atualizar UI
            this.moveCard(cardId, listId, position);
        });

        this.connection.on("CardDeleted", (cardId) => {
            console.log("Card deletado:", cardId);
            // Atualizar UI
            this.removeCard(cardId);
        });
    }

    // Métodos para manipular a UI
    addCardToList(card) {
        const list = document.querySelector(`[data-list-id="${card.listId}"]`);
        if (list) {
            const cardElement = this.createCardElement(card);
            list.querySelector('.cards-container').appendChild(cardElement);
        }
    }

    updateCardInList(card) {
        const cardElement = document.querySelector(`[data-card-id="${card.id}"]`);
        if (cardElement) {
            cardElement.querySelector('.card-title').textContent = card.title;
            cardElement.querySelector('.card-description').textContent = card.description;
        }
    }

    moveCard(cardId, listId, position) {
        const cardElement = document.querySelector(`[data-card-id="${cardId}"]`);
        const newList = document.querySelector(`[data-list-id="${listId}"]`);
        if (cardElement && newList) {
            const cardsContainer = newList.querySelector('.cards-container');
            const cards = Array.from(cardsContainer.children);
            const insertPosition = cards.findIndex(c => 
                parseInt(c.dataset.position) > position);
            
            if (insertPosition === -1) {
                cardsContainer.appendChild(cardElement);
            } else {
                cardsContainer.insertBefore(cardElement, cards[insertPosition]);
            }
            
            cardElement.dataset.position = position;
        }
    }

    removeCard(cardId) {
        const cardElement = document.querySelector(`[data-card-id="${cardId}"]`);
        if (cardElement) {
            cardElement.remove();
        }
    }

    createCardElement(card) {
        const div = document.createElement('div');
        div.className = 'card';
        div.dataset.cardId = card.id;
        div.dataset.position = card.position;
        div.innerHTML = `
            <h3 class="card-title">${card.title}</h3>
            <p class="card-description">${card.description || ''}</p>
            ${card.assignedUser ? `<span class="assigned-user">${card.assignedUser.name}</span>` : ''}
        `;
        return div;
    }

    // Métodos para interagir com o hub
    async joinBoard(boardId) {
        await this.connection.invoke("JoinBoard", boardId.toString());
    }

    async leaveBoard(boardId) {
        await this.connection.invoke("LeaveBoard", boardId.toString());
    }
}

// Inicializar cliente
const boardHub = new BoardHubClient();