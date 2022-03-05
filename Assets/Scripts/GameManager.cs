using UnityEngine;

public class GameManager : MonoBehaviour {
    private GameObject player;
    private SpriteRenderer spriteRenderer;
    private AnimatedSprites aniSprites;

    private void Awake() {
        player = GameObject.Find("Player");
        aniSprites = player.GetComponent(typeof(AnimatedSprites)) as AnimatedSprites;
        spriteRenderer = player.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
    }

    private void Start() {        
        loadPlayerSprites();
    }

    private void Update() {
        
    }

    private void loadPlayerSprites() {
        Sprite[] playerSprites = new Sprite[4];

        switch(PlayerStats.character) {
            case 2:
                spriteRenderer.sprite = Resources.Load<Sprite>("Kiara_Normal_01");

                playerSprites[0] = Resources.Load<Sprite>("Kiara_Normal_01");
                playerSprites[1] = Resources.Load<Sprite>("Kiara_Normal_02");
                playerSprites[2] = Resources.Load<Sprite>("Kiara_Normal_03");
                playerSprites[3] = Resources.Load<Sprite>("Kiara_Normal_02");

                aniSprites.sprites = playerSprites;
                break;
            case 3:
                spriteRenderer.sprite = Resources.Load<Sprite>("Ame_Normal_01");

                playerSprites[0] = Resources.Load<Sprite>("Ame_Normal_01");
                playerSprites[1] = Resources.Load<Sprite>("Ame_Normal_02");
                playerSprites[2] = Resources.Load<Sprite>("Ame_Normal_03");
                playerSprites[3] = Resources.Load<Sprite>("Ame_Normal_02");

                aniSprites.sprites = playerSprites;
                break;
            case 4:
                spriteRenderer.sprite = Resources.Load<Sprite>("Calli_Normal_01");

                playerSprites[0] = Resources.Load<Sprite>("Calli_Normal_01");
                playerSprites[1] = Resources.Load<Sprite>("Calli_Normal_02");
                playerSprites[2] = Resources.Load<Sprite>("Calli_Normal_03");
                playerSprites[3] = Resources.Load<Sprite>("Calli_Normal_02");

                aniSprites.sprites = playerSprites;
                break;
            case 5:
                spriteRenderer.sprite = Resources.Load<Sprite>("Gura_Normal_01");

                playerSprites[0] = Resources.Load<Sprite>("Gura_Normal_01");
                playerSprites[1] = Resources.Load<Sprite>("Gura_Normal_02");
                playerSprites[2] = Resources.Load<Sprite>("Gura_Normal_03");
                playerSprites[3] = Resources.Load<Sprite>("Gura_Normal_02");

                aniSprites.sprites = playerSprites;
                break;
            default:
                spriteRenderer.sprite = Resources.Load<Sprite>("Ina_Normal_01");

                playerSprites[0] = Resources.Load<Sprite>("Ina_Normal_01");
                playerSprites[1] = Resources.Load<Sprite>("Ina_Normal_02");
                playerSprites[2] = Resources.Load<Sprite>("Ina_Normal_03");
                playerSprites[3] = Resources.Load<Sprite>("Ina_Normal_02");

                aniSprites.sprites = playerSprites;
                break;
        }
    }
}
